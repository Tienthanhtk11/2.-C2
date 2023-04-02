import React, { useEffect, useState } from "react";
import { Button, Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import { EditOutlined } from "@ant-design/icons";
import ModifyUserModal from "./modifyUserModal";
import CreateUserModal from "./createUserModal";
import useSWR from "swr";
import { user } from "@/services/user";
import { fetcher } from "@/common/const";
import { getToken } from "@/common/function-global";

interface DataType {
  user_name: string;
  password: string;
  pass_code: string;
  email: string;
  name: string;
  id: number;
  dateAdded: Date;
}

export default function User() {
  const columns: ColumnsType<DataType> = [
    {
      title: "ID",
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
      key: "user_name",
      dataIndex: "user_name",
    },
    {
      title: "Action",
      key: "action",
      render: (_, record, index) => (
        <Space size="middle">
          <EditOutlined onClick={() => handleOpenModifyUserModal(record)} />
        </Space>
      ),
    },
  ];
  const initFilter = {
    user_name: "",
  };

  const [lstTable, setListTable] = useState([]);
  const [isVisibleCreateUserModal, setVisibleCreateUserModal] = useState(false);
  const [isVisibleModifyUserModal, setVisibleModifyUserModal] = useState(false);

  const [UserData, setUserData] = useState<any>();
  const [filterTable, setfilterTable] = useState<any>(initFilter);

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR([user().admin().list(filterTable), getToken()], ([url, token]) => fetcher(url, token));
  
  useEffect(() => {
    if (listRes && !error) {
      setListTable(listRes?.data);
    }
  }, [error, listRes]);

  // thêm
  const handleCreateUserModalClose = (res: any) => {
    setVisibleCreateUserModal(false);
    if (res) {
      mutate({ ...listRes?.data?.list, res });
    }
  };

  const handleOpenCreateUserModal = () => {
    setVisibleCreateUserModal(true);
  };

  // sửa
  const handleModifyUserModal = (res: any) => {
    setVisibleModifyUserModal(false);
    if (res) {
      listRes?.data?.list.find((obj: any) => {
        if (obj.id == res.id) {
          return (obj = res);
        }
      });
    }
  };

  const handleOpenModifyUserModal = (data: any) => {
    setUserData(data);
    setVisibleModifyUserModal(true);
  };
  return (
    <>
      <Button
        type="primary"
        className="bg-blue-500"
        onClick={() => handleOpenCreateUserModal()}
      >
        Thêm mới
      </Button>

      <Table columns={columns} dataSource={lstTable ?? []} />

      {isVisibleCreateUserModal && (
        <CreateUserModal
          show={isVisibleCreateUserModal}
          handleCreateUserModalClose={handleCreateUserModalClose}
        />
      )}
      {isVisibleModifyUserModal && (
        <ModifyUserModal
          show={isVisibleModifyUserModal}
          data={UserData}
          handleModifyUserModal={handleModifyUserModal}
        />
      )}
    </>
  );
}
