import React, { useEffect, useState } from "react";
import { Space, Table } from "antd";
import type { ColumnsType } from "antd/es/table";
import { DeleteOutlined, EditOutlined, EyeOutlined } from "@ant-design/icons";
import useSWR from "swr";
import { order } from "@/services/order";
import ModifyOrderModal from "./ModifyOrderModal";
import DeleteOrderModal from "./DeleteOrderModal";
import { fetcher } from "@/common/const";
import ViewOrderModal from "./ViewOrderModal";
import { getToken } from "@/common/function-global";

interface DataType {
  code: string;
  id: number;
  customer_id: number;
  count_sms: number;
  status: number;
  timeSend: Date;
}
export default function Order(props: any) {
  const columns: ColumnsType<DataType> = [
    {
      title: "ID",
      dataIndex: "id",
      key: "id",
    },
    {
      title: "Mã đơn hàng",
      dataIndex: "code",
      key: "code",
    },
    {
      title: "ID khách hàng",
      dataIndex: "customer_id",
      key: "customer_id",
    },
    {
      title: "Số tin nhắn",
      dataIndex: "count_sms",
      key: "count_sms",
    },
    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
    },
    {
      title: "Thời gian gửi",
      dataIndex: "timeSend",
      key: "timeSend",
    },
    {
      title: "Action",
      key: "action",
      render: (_, record, index) => (
        <Space size="middle">
          <EyeOutlined onClick={() => handleOpenViewOrderModal(record)} />
          <EditOutlined onClick={() => handleOpenModifyOrderModal(record)} />
          <DeleteOutlined
            onClick={() => handleOpenDeleteOrderModal(record, index)}
          />
        </Space>
      ),
    },
  ];

  const [lstTable, setListTable] = useState([]);
  const [orderData, setOrderData] = useState<any>();

  const [isVisibleDeleteOrderModal, setVisibleDeleteOrderModal] =
    useState(false);

  const [isVisibleModifyOrderModal, setVisibleModifyOrderModal] =
    useState(false);

  const [isVisibleViewOrderModal, setVisibleViewOrderModal] =
    useState(false);

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR([order().list(), getToken()], ([url, token]) => fetcher(url, token));  

  useEffect(() => {
    if (listRes && !error) {
      setListTable(listRes.data);
    }
  }, [error, listRes]);

  // cập nhật
  const handleModifyOrderModal = (res: any) => {
    setVisibleModifyOrderModal(false);
    if (res) {
      mutate({ ...lstTable, res });
    }
  };
  const handleOpenModifyOrderModal = (data: any) => {
    setOrderData(data);
    setVisibleModifyOrderModal(true);
  };

  // xoá
  const handleDeleteOrderModal = (index: number) => {
    setVisibleDeleteOrderModal(false);
    if (!!index) {
      listRes.data.splice(index, 1);
    }
  };
  const handleOpenDeleteOrderModal = (data: any, index: number) => {
    setOrderData({ ...data, index: index });
    setVisibleDeleteOrderModal(true);
  };

  // xem thông tin
  const handleViewOrderModal = (res: any) => {
    setVisibleViewOrderModal(false);
  };
  const handleOpenViewOrderModal = (data: any) => {
    setOrderData(data);
    setVisibleViewOrderModal(true);
  };

  return (
    <>
      <Table columns={columns} dataSource={lstTable ?? []} />

      {isVisibleModifyOrderModal && (
        <ModifyOrderModal
          show={isVisibleModifyOrderModal}
          data={{ id: orderData.id }}
          handleModifyOrderModal={handleModifyOrderModal}
        />
      )}
      {isVisibleDeleteOrderModal && (
        <DeleteOrderModal
          show={isVisibleDeleteOrderModal}
          data={{ id: orderData.id, index: orderData.index }}
          handleDeleteOrderModal={handleDeleteOrderModal}
        />
      )}
      {isVisibleViewOrderModal && (
        <ViewOrderModal
          show={isVisibleViewOrderModal}
          data={{ id: orderData.id }}
          handleViewOrderModal={handleViewOrderModal}
        />
      )}
    </>
  );
}
