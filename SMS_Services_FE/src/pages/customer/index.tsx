import React, { useEffect, useState } from "react";
import { Button, Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR from "swr";
import { customer } from "@/services/customer";
import { fetcher } from "@/common/const";
import { getToken } from "@/common/function-global";

interface DataType {
  name: string;
  email: string;
  cash: 0;
  active: boolean;
  id: number;
  dateAdded: Date;
  last_active: Date;

}

export default function Customer() {
  const columns: ColumnsType<DataType> = [
    {
      title: "Id",
      dataIndex: "id",
      key: "id",
    },
    {
      title: "Customer Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "User Name",
      dataIndex: "user_name",
      key: "user_name",
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
    },
    {
      title: "Cash",
      key: "cash",
      dataIndex: "cash",
    },
    {
      title:"Last app work",
      key: "last_active",
      dataIndex: "last_active",
    },
  ];
  const initFilter = {
    user_name: "",
  };

  const [lstTable, setListTable] = useState([]);
  const [CustomerData, setCustomerData] = useState<any>();
  const [filterTable, setfilterTable] = useState<any>(initFilter);

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR([customer().customer().list(filterTable), getToken()], ([url, token]) => fetcher(url, token));  
  useEffect(() => {
    if (listRes && !error) {
      setListTable(listRes?.data);
    }
  }, [error, listRes]);

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />
    </>
  );
}
