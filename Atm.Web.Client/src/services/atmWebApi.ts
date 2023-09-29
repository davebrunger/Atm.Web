import { DatesAsStrings } from "../models/DatesAsStrings";
import { Transaction } from "../models/Transaction";

type AtmWebCallParams = {
    readonly method: string,
    readonly url: string,
    readonly token?: string,
    readonly body?: unknown,
}

type AtmWebFetchParams<TResponse, TResult> = AtmWebCallParams & {
    readonly selector: (response: TResponse) => TResult
}

async function atmWebCall(params: AtmWebCallParams): Promise<Response | undefined> {
    const response = await fetch(`https://localhost:7025/api/${params.url}`, {
        method: params.method,
        body: params.body ? JSON.stringify(params.body) : undefined,
        headers: {
            ...params.body ? { "Content-Type": "application/json" } : {},
            ...params.token ? { "Authorization": `Bearer ${params.token}` } : {}
        }
    });
    if (!response.ok) {
        alert(response.status);
        return undefined;
    }
    return response;
}

async function atmWebFetch<TResponse, TResult>(params: AtmWebFetchParams<TResponse, TResult>): Promise<TResult | undefined> {
    const response = await atmWebCall(params);
    if (!response) {
        return undefined;
    }
    var json = await response.json() as TResponse;
    return params.selector(json);
}

export function getBearerToken(accountNumber: string, pin: string): Promise<string | undefined> {
    return atmWebFetch({
        method: "POST",
        url: "Accounts/login",
        selector: (json: { token: string }) => json.token,
        body: {
            accountNumber: accountNumber,
            pin: pin
        }
    });
}

export function getUserBalance(token: string): Promise<number | undefined> {
    return atmWebFetch({
        method: "GET",
        url: "Accounts/balance",
        selector: (json: { balance: number }) => json.balance,
        token: token
    });
}

export async function makeUserWithdrawal(token: string, withdrawalAmount: number): Promise<void> {
    await atmWebCall({
        method: "POST",
        url: "Transactions/withdrawal",
        body: {
            amount: withdrawalAmount
        },
        token: token
    });
};

export function getLastFiveUserTransactions(token: string): Promise<Transaction[] | undefined> {
    return atmWebFetch<DatesAsStrings<Transaction>[], Transaction[]>({
        method: "GET",
        url: "Transactions/lastFive",
        token: token,
        selector : a => a.map(t => ({...t, dateTime : new Date(t.dateTime)}))
    });
};


