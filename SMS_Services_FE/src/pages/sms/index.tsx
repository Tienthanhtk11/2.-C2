import React, { useEffect, useState } from "react";
import { Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR from "swr";
import { sms } from "@/services/sms";
import { fetcher } from "@/common/const";

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

  const [lstTable, setListTable] = useState([]);
  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR(sms().list(), fetcher);

  useEffect(() => {
    if (listRes && !error) {
      setListTable(listRes.data);
    }
  }, [error, listRes]);

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />
    </>
  );
}
