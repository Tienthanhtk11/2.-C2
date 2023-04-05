import React, { useEffect, useState } from "react";
import { Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR from "swr";
import { sms } from "@/services/sms";
import { fetcher } from "@/common/const";
import { getToken, sendRequest_$GET } from "@/common/function-global";
import { useSelector } from "react-redux";
import { userType } from "@/common/enum";
import useSWRMutation from "swr/mutation";

interface DataType {
  date_receive: string;
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
    },
    {
      title: "Thời gian nhận",
      dataIndex: "date_receive",
      key: "date_receive",
    }
  ];

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

  // const {
  //   data: listResAdmin,
  //   error: errorResAdmin,
  //   mutate: mutateAdmin,
  // } = useSWR([sms().list_admin(), getToken()], ([url, token]) => fetcher(url, token));

  // const {
  //   data: listResCustomer,
  //   error: errorResCustomer,
  //   mutate: mutateCustomer,
  // } = useSWR([sms().list(), getToken()], ([url, token]) => fetcher(url, token));

  const {
    trigger: triggerAdmin,
    data: listResAdmin,
    error: errorResAdmin,
  } = useSWRMutation(sms().list_admin(), sendRequest_$GET);

  const {
    trigger: triggerCustomer,
    data: listResCustomer,
    error: errorResCustomer,
  } = useSWRMutation(sms().list(), sendRequest_$GET);

  useEffect(() => {
    // if (listResAdmin && !errorResAdmin) {
    //   setListTable(listResAdmin.data);
    // }
    // if (listResCustomer && !errorResCustomer) {
    //   setListTable(listResCustomer.data);
    // }
    if (getStore.typeuser == userType.admin) {
      if (listResAdmin && !errorResAdmin) {
        triggerAdmin();
        setListTable(listResAdmin.data);
      }
    }
    else {
      if (listResCustomer && !errorResCustomer) {
        triggerCustomer()
        setListTable(listResCustomer.data);
      }
    }
  }, [errorResAdmin, listResAdmin, errorResCustomer, listResCustomer]);

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />
    </>
  );
}
