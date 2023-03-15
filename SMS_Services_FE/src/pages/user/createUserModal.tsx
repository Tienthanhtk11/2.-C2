import { Checkbox, DatePicker, Form, Input, Modal } from "antd";
import { sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect } from "react";
import { user } from "@/services/user";
import useSWRMutation from "swr/mutation";

const { Item } = Form;

type Props = {
  show: boolean;
  handleCreateUserModalClose: any;
};

const CreateUserModal: React.FC<Props> = (props) => {
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
      props.handleCreateUserModalClose(adminData.data);
    }
  }, [adminData, props]);

  return (
    <>
      <Modal
        title={"Thêm Tài Khoản"}
        centered
        open={props.show}
        onOk={() => {
          form.submit();
        }}
        onCancel={props.handleCreateUserModalClose}
        okButtonProps={{
          className: "bg-blue-500",
        }}
        okText="Xác nhận"
        cancelText="Thoát"
      >
        <Form
          layout="vertical"
          form={form}
          onFinish={handleFormSubmit}
          initialValues={initialValuesForm}
        >
          <Item name="full_name" label="Tên đầy đủ">
            <Input />
          </Item>

          <Item name="code" label="Mã ">
            <Input />
          </Item>

          <Item name="username" label="Tên tài khoản">
            <Input />
          </Item>

          <Item name="password" label="Mật khẩu">
            <Input />
          </Item>

          <Item name="address" label="Địa chỉ">
            <Input />
          </Item>

          <Item name="birthday" label="Ngày sinh">
            <DatePicker className="w-full" />
          </Item>

          <Item name="email" label="Email">
            <Input />
          </Item>

          <Item name="phone_number" label="Số điện thoại">
            <Input />
          </Item>

          <Item name="is_active" label="Trạng thái" valuePropName="checked">
            <Checkbox />
          </Item>
        </Form>
      </Modal>
    </>
  );
};

export default CreateUserModal;
