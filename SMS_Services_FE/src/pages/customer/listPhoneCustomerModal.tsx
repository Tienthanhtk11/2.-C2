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
      title: "Số điện thoại",
      dataIndex: "phone_number",
      key: "phone_number",
    },
  ];

  const [lstTable, setListTable] = useState([]);
  const [CustomerData, setCustomerData] = useState<any>();
  const [isVisibleCreatePhoneNumberModal, setVisibleCreatePhoneNumberModal] = useState(false);
  const [selectedRowKeys, setSelectedRowKeys] = useState<any>()

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR([(props.data?.id) || (isVisibleCreatePhoneNumberModal) ? customer().customer().listphonenumber(props.data.id) : '', getToken()], ([url, token]) => sendRequest_$GET(url, token));

  const {
    data: listPhoneAll,
    error: errorPhoneAll,
  } = useSWR([(!!listRes) ? customer().customer().listallphonenumber() : '', getToken()], ([url, token]) => sendRequest_$GET(url, token));

  useEffect(() => {
    if (listRes && !error) {
      listRes.data?.map((obj: any, index: number) => obj.key = index + 1);
      setListTable(listRes?.data);
    }
    if (listPhoneAll && !errorPhoneAll) {
      listPhoneAll?.data.map((obj: any, index: number) => obj.key = index + 1);
      if (!!lstTable) {
        console.log("vào");
        let checked: any = [];
        let lstTable_copy = lstTable;
        // console.log('lstTable_copy :', lstTable_copy);
        // console.log('listPhoneAll :', listPhoneAll.data);
        // debugger
        lstTable_copy.forEach((obj: any) => {
          listPhoneAll?.data.forEach((element: any) => {    
            element.id == obj.phone_id && checked.push(element.key)
          });
        });
        setSelectedRowKeys(checked);
      }
      // setSelectedRowKeys(listPhoneAll.data?.find((item: any) => item.id == manageClassData.teacher_id).key)
    }
  }, [error, listRes, props, listPhoneAll, errorPhoneAll]);

  const rowSelection = {

    onChange: (selectedRowKeys: React.Key[], selectedRows: DataType[]) => {

      setCustomerData(selectedRows[0])
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
      mutate({ ...listRes.data, res });
    }
  };

  const handleOpenCreatePhoneNumberModal = () => {
    setVisibleCreatePhoneNumberModal(true);
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
        <Table columns={columns} dataSource={lstTable ?? []} />
      </Modal>
      {isVisibleCreatePhoneNumberModal && (
        <CreatePhoneNumberModal
          show={isVisibleCreatePhoneNumberModal}
          data={{ customer_id: props.data?.id }}
          handleCreatePhoneNumberModal={handleCreatePhoneNumberModalClose}
          selectedRowKeys={selectedRowKeys}
          listPhoneAll={listPhoneAll?.data}
        />
      )}
    </>
  );
};

export default ListPhoneCustomerModal;
