import { Form, Input, Modal } from "antd";
import { sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect } from "react";
import { user } from "@/services/user";
import useSWRMutation from "swr/mutation";

const { Item } = Form;

type Props = {
  show: boolean;
  handleCreateCustomerModalClose: any;
};

const CreateCustomerModal: React.FC<Props> = (props) => {
  const [form] = Form.useForm();
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
        <Form
          layout="vertical"
          form={form}
          onFinish={handleFormSubmit}
          initialValues={initialValuesForm}
        >
          <Item name="phone" label="SĐT">
            <Input />
          </Item>

          <Item name="name" label="Tên Đầy Đủ">
            <Input />
          </Item>

          <Item name="password" label="Mật khẩu">
            <Input />
          </Item>

          <Item name="customer_affliate" label="customer_affliate">
            <Input />
          </Item>
        </Form>
      </Modal>
    </>
  );
};

export default CreateCustomerModal;
