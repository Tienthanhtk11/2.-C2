import { Form, Input, Modal, Space, Table } from "antd";
import { sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect } from "react";
import { user } from "@/services/user";
import useSWRMutation from "swr/mutation";
import { ColumnsType } from "antd/es/table";
import { EditOutlined } from "@ant-design/icons";

const { Item } = Form;

type Props = {
  show: boolean;
  handleCreateCustomerModalClose: any;
  data:any
};

const ListPhoneCustomerModal: React.FC<Props> = (props) => {
  const [form] = Form.useForm();

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

  const initialValuesForm = {
    is_active: true,
  };

  const {
    trigger,
    data: adminData,
    error,
  } = useSWRMutation(user().admin().create, sendRequest_$POST);

  const handleFormSubmit = async (values: any) => {
    trigger({
      ...values,
      id: 0,
      is_delete: false,
      dateAdded: dayjs(),
      userAdded: 0,
      type: 0,
    });
  };

  useEffect(() => {
    if (adminData?.data) {
      props.handleCreateCustomerModalClose(adminData.data);
    }
  }, [adminData, props]);

  return (
    <>
      <Modal
        title={"Đăng Ký"}
        centered
        open={props.show}
        onOk={() => {
          form.submit();
        }}
        onCancel={props.handleCreateCustomerModalClose}
        okButtonProps={{
          className: "bg-blue-500",
        }}
        okText="Đăng Ký"
        cancelText="Hủy"
      >
       <Table columns={columns} dataSource={lstTable ?? []} />
      </Modal>
    </>
  );
};

export default ListPhoneCustomerModal;
