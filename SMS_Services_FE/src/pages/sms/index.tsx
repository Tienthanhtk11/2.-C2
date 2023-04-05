import React, { useEffect, useState } from "react";
import { Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR, { useSWRConfig } from "swr";
import { sms } from "@/services/sms";
import { fetcher } from "@/common/const";
import { getToken, sendRequest_$GET } from "@/common/function-global";
import { useSelector } from "react-redux";
import { userType } from "@/common/enum";
import useSWRMutation from "swr/mutation";

interface DataType {
  date_receive: Date;
  status: number;
  message: number;
  phone_send: number;
  phone_receive: number;
  timeSend: Date;
  id: number;
}
export default function SMS(props: any) {
  const columns: ColumnsType<DataType> = [
    {
      title: "ID",
      dataIndex: "id",
      key: "id",
    },
    {
      title: "Phone Receive",
      dataIndex: "phone_receive",
      key: "phone_receive",
    },
    {
      title: "Phone Send",
      dataIndex: "phone_send",
      key: "phone_send",
    },
    {
      title: "Nội dung tin nhắn",
      dataIndex: "message",
      key: "message",
      render: (text, record, index) => <div className="max-length" >{record.message}</div>
    },
    {
      title: "Thời gian nhận",
      dataIndex: "date_receive",
      key: "date_receive",
    }
  ];

  const { mutate } = useSWRConfig();

  // get user admin
  const getStore = useSelector(
    (state: any) => {
      return {
        admin: state.infoCurrentUserAminReducers,
        customer: state.infoCurrentUserReducers,
        typeuser: state.typeUserReducers
      }
    }
  );

  const [lstTable, setListTable] = useState([]);
  const [getaaa, setaaa] = useState('');
  const [getbbb, setbbb] = useState('');

  const {
    data: listResAdmin,
    error: errorResAdmin,
    mutate: mutateAdmin,
  } = useSWR([getaaa, getToken()], ([url, token]) => fetcher(url, token));

  const {
    data: listResCustomer,
    error: errorResCustomer,
    mutate: mutateCustomer,
  } = useSWR([getbbb, getToken()], ([url, token]) => fetcher(url, token));

  useEffect(() => {
    if (getStore.typeuser == userType.admin) {
      // mutateAdmin(`${sms().list_admin()}`)
      setaaa(`${sms().list_admin()}`)
    }
    else {
      setbbb(`${sms().list()}`)
    }
  }, []);

  useEffect(() => {
    if (listResAdmin && !errorResAdmin) {
      setListTable(listResAdmin.data);
    }
    if (listResCustomer && !errorResCustomer) {
      setListTable(listResCustomer.data);
    }
  }, [errorResAdmin, listResAdmin, errorResCustomer, listResCustomer]);

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />
    </>
  );
}
