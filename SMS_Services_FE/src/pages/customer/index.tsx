import React, { useEffect, useState } from "react";
import { Button, Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR from "swr";
import { customer } from "@/services/customer";
import { fetcher } from "@/common/const";

interface DataType {
  name: string;
  email: string;
  cash: 0;
  active: boolean;
  id: number;
  dateAdded: Date;
}

export default function Customer() {
  const columns: ColumnsType<DataType> = [
    {
      title: "Id",
      dataIndex: "id",
      key: "id",
    },
    {
      title: "Tên",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Tên đăng nhập",
      dataIndex: "user_name",
      key: "user_name",
    },
    {
      title: "email",
      dataIndex: "email",
      key: "email",
    },
    {
      title: "cash",
      key: "cash",
      dataIndex: "cash",
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
  } = useSWR(customer().customer().list(filterTable), fetcher);

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
