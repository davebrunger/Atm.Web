import React from "react";
import { Table } from "reactstrap";
import { getLastFiveUserTransactions } from "../services/atmWebApi";
import { Transaction } from "../models/Transaction";
import { TransactionRow } from "./TransactionRow";

interface Props {
    readonly token: string;
    readonly balance: number;
}

export function LastFiveTransactions(props: Props) {

    const [transactions, setTransactions] = React.useState<Transaction[] | undefined>();

    React.useEffect(() => {
        (async() => {
            const transactions = await getLastFiveUserTransactions(props.token);
            setTransactions(transactions);
        })();
    }, [props.token, props.balance]);

    return (
        <Table>
            <thead>
                <th>Transaction Number</th>
                <th className="text-end">Date and Time</th>
                <th className="text-end">Amount</th>
            </thead>
            <tbody>
                {transactions && transactions.map(t => <TransactionRow key={t.id} transaction={t} />)}
                {!transactions && <tr><td colSpan={3}>Please Wait...</td></tr>}
            </tbody>
        </Table>
    );
}