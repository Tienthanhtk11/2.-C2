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

const CreatePhoneNumberModal: React.FC<Props> = (props) => {
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
  ];

  
  const [lstTable, setListTable] = useState([]);
  const [selectPhoneData, setSelectPhoneData] = useState<any>();

  const {
    data: listRes,
    error,
    isLoading,
    mutate,
  } = useSWR((!!props.data.getId) ? [customer().customer().listphonenumber(props.data.getId), getToken()]: null, ([url, token]) => fetcher(url, token));  

  const {
    trigger,
    data: createphonenumberData,
    error: createphonenumberError,
  } = useSWRMutation(customer().customer().createphonenumber, sendRequest_$POST);

  const handleFormSubmit = async () => {
    await trigger({ ...selectPhoneData });
  };

  useEffect(() => {
    if (createphonenumberData?.statusCode == statusCode.OK) {
      notificationSuccess("Thêm mới thành công");
      props.handleCreatePhoneNumberModal(createphonenumberData?.data);
    }
    else if (createphonenumberData?.statusCode == statusCode.Error) {
      notificationError(`${createphonenumberData?.message}`);
    }
    if (error) {
      notificationError(`${error}`);
    }
  }, [createphonenumberData, createphonenumberError]);

  const Cancel = () => {
    props.handleCreatePhoneNumberModal();
  }

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
        <Table columns={columns} dataSource={lstTable ?? []} />
      </Modal>
    </>
  );
};
export default CreatePhoneNumberModal;
