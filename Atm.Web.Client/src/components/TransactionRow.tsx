import { Transaction } from "../models/Transaction";

interface Props {
    readonly transaction : Transaction;
}

export function TransactionRow(props : Props)
{
    return (
        <tr>
            <td>{props.transaction.id}</td>
            <td className="text-end">{props.transaction.dateTime.toDateString()}{' '}{props.transaction.dateTime.toLocaleTimeString()}</td>
            <td className="text-end">{props.transaction.amount}</td>
        </tr>
    );
}