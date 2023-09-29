import React from "react";
import { Card, CardBody, Col, Form, FormGroup, Input, Label, Nav, NavItem, NavLink, Row } from "reactstrap";
import { getUserBalance } from "../services/atmWebApi";
import { Withdrawal } from "./Withdrawal";
import { LastFiveTransactions } from "./LastFiveTransactions";

interface Props {
    readonly accountNumber: string;
    readonly token: string;
}

export function AccountDetails(props: Props) {

    const [balance, setBalance] = React.useState<number | undefined>();
    const [tab, setTab] = React.useState<"WITHDRAWAL" | "TRANSACTIONS">("WITHDRAWAL");

    const getBalance = React.useCallback(async () => {
        const balance = await getUserBalance(props.token);
        setBalance(balance);
    }, [props.token]);

    React.useEffect(() => { getBalance(); }, [getBalance]);

    return (
        <>
            <Form>
                <Row>
                    <Col>
                        <FormGroup>
                            <Label for="accountNumber">Account Number</Label>
                            <Input plaintext value={props.accountNumber} readOnly />
                        </FormGroup>
                    </Col>
                    <Col>
                        <FormGroup>
                            <Label for="balance">Balance</Label>
                            <Input plaintext value={balance ?? "Not Requested"} readOnly />
                        </FormGroup>
                    </Col>
                </Row>
            </Form>
            <Nav tabs>
                <NavItem>
                    <NavLink active={tab === "WITHDRAWAL"} onClick={() => setTab("WITHDRAWAL")} href="#">
                        Make Withdrawal
                    </NavLink>
                </NavItem>
                <NavItem>
                    <NavLink active={tab === "TRANSACTIONS"} onClick={() => setTab("TRANSACTIONS")} href="#">
                        Last Five Transactions
                    </NavLink>
                </NavItem>
            </Nav>
            <Card className="border-top-0" style={{ borderTopLeftRadius: 0, borderTopRightRadius: 0, WebkitBorderTopLeftRadius: 0, WebkitBorderTopRightRadius: 0 }}>
                <CardBody>
                    {tab === "WITHDRAWAL" && <Withdrawal token={props.token} withdrawalSuccessful={() => getBalance()} />}
                    {tab === "TRANSACTIONS" && <LastFiveTransactions token={props.token} balance={balance ?? 0} />}
                </CardBody>
            </Card>
        </>
    );
}
