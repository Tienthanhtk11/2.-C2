import { Button, Form, Input, Modal, Space, Table } from "antd";
import { getToken, sendRequest_$GET, sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import React, { useEffect, useState } from "react";
import { user } from "@/services/user";
import useSWRMutation from "swr/mutation";
import { ColumnsType } from "antd/es/table";
import { EditOutlined } from "@ant-design/icons";
import useSWRWithFallbackData from "@/common/use-swr-with-fallback-data";
import { fetcher } from "@/common/const";
import { customer } from "@/services/customer";
import CreatePhoneNumberModal from "./createPhoneNumberModal";
import useSWR from "swr";

type Props = {  
  show: boolean;
  handleListPhoneCustomerModalClose: any;
  data: any
};

interface DataType {
  key: React.Key;
  code: string;
  id: number;
  customer_id: number;
  count_sms: number;
  status: number;
  timeSend: Date;
}

const ListPhoneCustomerModal: React.FC<Props> = (props) => {
  const columns: ColumnsType<DataType> = [
    {
      title: "STT",
      render: (_, record, index) => (
        <div className="center">{index + 1}</div>
      ),
    },
    {
      title: "Mã",
      dataIndex: "code",
      key: "code",
    },
    {
      title: "Tên",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Thao tác",
      key: "action",
      render: (_, record, index) => (
        <Space size="middle">
          {/* <EditOutlined
              onClick={() => handleOpenModifyManageClassModal(record)}
            />      */}
        </Space>
      ),
    },
  ];

  const [lstTable, setListTable] = useState([]);
  const [phoneData, setPhoneData] = useState<any>();
  const [isVisibleCreatePhoneNumberModal, setVisibleCreatePhoneNumberModal] = useState(false);

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR((props.show) ? customer().customer().listphonenumber(props.data.getId): null, sendRequest_$GET);  

  useEffect(() => {
    if (listRes && !error) {
      listRes.data?.map((obj: any, index: number) => obj.key = index + 1);
      setListTable(listRes?.data);
    }
  }, [error, listRes]);

  const rowSelection = {

    onChange: (selectedRowKeys: React.Key[], selectedRows: DataType[]) => {

      setPhoneData(selectedRows[0])
    },
    onselect: (record: any, selected: boolean, selectedRows: any, nativeEvent: Event) => {
      // console.log(record);
      // console.log(selected);
      // console.log(selectedRows);
      // console.log(nativeEvent);
    },
    getCheckboxProps: (record: DataType) => ({
      // disabled: record.username === 'Disabled User', // Column configuration not to be checked
      // name: record.username,
    }),
  };

  // thêm
  const handleCreatePhoneNumberModalClose = (res: any) => {
    setVisibleCreatePhoneNumberModal(false);
    if (res) {
      mutate({ ...listRes?.data?.list, res });
    }
  };

  const handleOpenCreatePhoneNumberModal = () => {
    setVisibleCreatePhoneNumberModal(true);
    setPhoneData
  };

  return (
    <>
      <Modal
        title={"Danh sách số điện thoại khách quản lý"}
        centered
        open={props.show}
        // onOk={() => {
        //   form.submit();
        // }}
        onCancel={props.handleListPhoneCustomerModalClose}
        okButtonProps={{ style: { display: 'none' } }}
        // okText="Đăng Ký"
        cancelText="Hủy"
        width={1000}
      >
        <Button
          type="primary"
          className="bg-blue-500"
          onClick={() => handleOpenCreatePhoneNumberModal()}
        >
          Thêm mới
        </Button>
        <Table columns={columns} dataSource={lstTable ?? []} rowSelection={{
          type: 'radio',
          ...rowSelection
        }} />
      </Modal>
      {isVisibleCreatePhoneNumberModal && (
        <CreatePhoneNumberModal
          show={isVisibleCreatePhoneNumberModal}
          data={{ id: phoneData.id }}
          handleCreatePhoneNumberModal={handleCreatePhoneNumberModalClose}
        />
      )}
    </>
  );
};

export default ListPhoneCustomerModal;
