import { Form, Modal, Table } from "antd";
import { deepCopy, getToken, notificationError, notificationSuccess, sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { customer } from "@/services/customer";
import useSWRMutation from "swr/mutation";
import { statusCode } from "@/common/enum";
import useSWRWithFallbackData from "@/common/use-swr-with-fallback-data";
import { fetcher } from "@/common/const";
import { ColumnsType } from "antd/es/table";
import useSWR from "swr";

const { Item } = Form;

type Props = {
  show: boolean;
  handleCreatePhoneNumberModal: any;
  data: any;
  selectedRowKeys: any;
  listPhoneAll: Array<any>;
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

const CreatePhoneNumberModal: React.FC<Props> = (props) => {
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


  const [lstTable, setListTable] = useState<Array<any>>([]);
  const [selectPhoneData, setSelectPhoneData] = useState<any>();

  const {
    trigger,
    data: createphonenumberData,
    error: createphonenumberError,
  } = useSWRMutation(customer().customer().createphonenumber, sendRequest_$POST);

  const handleFormSubmit = async () => {       
    let data = selectPhoneData.map((obj: any) => {
      let res = {
        id: 0,
        is_delete: false,
        customer_id: props.data.customer_id,
        phone_number: obj.phone_number,
        phone_id: obj.id,
      }
      return res;
    });
    await trigger(data);
  };

  useEffect(() => {
    if (props.listPhoneAll) {
      setListTable(props.listPhoneAll);
    }
    if (createphonenumberData?.statusCode == statusCode.OK) {
      notificationSuccess("Thêm mới thành công");
      props.handleCreatePhoneNumberModal(createphonenumberData?.data);
    }
    else if (createphonenumberData?.statusCode == statusCode.Error) {
      notificationError(`${createphonenumberData?.message}`);
    }
    if (createphonenumberError) {
      notificationError(`${createphonenumberError}`);
    }
  }, [createphonenumberData, createphonenumberError]);

  const Cancel = () => {
    props.handleCreatePhoneNumberModal();
  }

  const rowSelection = {

    onChange: (selectedRowKeys: React.Key[], selectedRows: DataType[]) => {
      setSelectPhoneData(selectedRows)
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

  return (
    <>
      <Modal
        title={"Cập Nhật Tài Khoản"}
        centered
        open={props.show}
        onOk={handleFormSubmit}
        onCancel={Cancel}
        okButtonProps={{
          className: "bg-blue-500",
        }}
        okText="Xác nhận"
        cancelText="Thoát"
      >
        <Table columns={columns} dataSource={lstTable ?? []} rowSelection={{
          defaultSelectedRowKeys: props.selectedRowKeys,
          type: 'checkbox',
          ...rowSelection
        }} />
      </Modal>
    </>
  );
};
export default CreatePhoneNumberModal;
