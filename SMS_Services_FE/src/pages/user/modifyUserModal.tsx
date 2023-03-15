import { Checkbox, DatePicker, Form, Input, Modal } from "antd";
import { sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { user } from "@/services/user";
import useSWRMutation from "swr/mutation";

const { Item } = Form;

type Props = {
  show: boolean;
  handleModifyUserModal: any;
  data: any;
};

const ModifyUserModal: React.FC<Props> = (props) => {
  const [form] = Form.useForm();

  const [getModifyUserData, setModifyUserData] = useState<any>(props.data);
  const [getId, setid] = useState<number>(0);

  const {
    trigger,
    data: adminData,
    error: adminError,
  } = useSWRMutation(user().admin().modify, sendRequest_$POST);

  const handleFormSubmit = async (values: any) => {
    trigger({ ...getModifyUserData, ...values });
  };

  useEffect(() => {
    if (getModifyUserData) {
      form.setFieldsValue({
        ...getModifyUserData,
        birthday: dayjs(getModifyUserData.birthday),
      });
    }
    if (adminData?.data) {
      props.handleModifyUserModal(adminData.data);
    }
  }, [form, getModifyUserData, adminData]);

  return (
    <>
      <Modal
        title={"Cập Nhật Tài Khoản"}
        centered
        open={props.show}
        onOk={() => {
          form.submit();
        }}
        onCancel={props.handleModifyUserModal}
        okButtonProps={{
          className: "bg-blue-500",
        }}
        okText="Xác nhận"
        cancelText="Thoát"
      >
        <Form layout="vertical" form={form} onFinish={handleFormSubmit}>
          <Item name="full_name" label="Tên đầy đủ">
            <Input />
          </Item>

          <Item name="code" label="Mã ">
            <Input />
          </Item>

          <Item name="username" label="Tên tài khoản">
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
export default ModifyUserModal;
