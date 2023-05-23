import React, { useEffect, useState } from "react";
import { Button, Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import useSWR from "swr";
import { customer } from "@/services/customer";
import { fetcher } from "@/common/const";
import { getToken } from "@/common/function-global";
import { EditOutlined } from "@ant-design/icons";
import ListPhoneCustomerModal from "./listPhoneCustomerModal";

interface DataType {
  name: string;
  email: string;
  cash: 0;
  active: boolean;
  id: number;
  dateAdded: Date;
  last_active: Date;
  status: string;
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
      title: "Status",
      key: "status",
      dataIndex: "status",
    },
    {
      title: "Last Connect",
      key: "last_active",
      dataIndex: "last_active",
    },
    {
      title: "Thao tác",
      key: "action",
      render: (_, record, index) => (
        <Space size="middle">
          <EditOutlined
            onClick={() => handleOpenModifyCustomerModal(record)}
          />
        </Space>
      ),
    },
  ];
  const initFilter = {
    user_name: "",
  };

  const [lstTable, setListTable] = useState([]);
  const [CustomerData, setCustomerData] = useState<any>();
  const [filterTable, setfilterTable] = useState<any>(initFilter);
  const [isVisibleListPhoneCustomerModal, setVisibleListPhoneCustomerModal] = useState(false);

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

  // danh sách tài khoản khách hàng
  const handleCustomerModalModal = (res: any) => {
    setVisibleListPhoneCustomerModal(false);
    if (res) {
      mutate({ ...listRes.data, res });
    }
  };

  const handleOpenModifyCustomerModal = (data: any) => {
    setCustomerData(data);
    setVisibleListPhoneCustomerModal(true);
  };

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />
      {isVisibleListPhoneCustomerModal && (
        <ListPhoneCustomerModal
          show={isVisibleListPhoneCustomerModal}
          data={{ id: CustomerData.id }}
          handleListPhoneCustomerModalClose={handleCustomerModalModal}
        />
      )}
    </>
  );
}
