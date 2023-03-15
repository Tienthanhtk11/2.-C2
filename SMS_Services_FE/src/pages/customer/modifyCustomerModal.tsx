import { DatePicker, Form, Input, Modal } from "antd";
import { deepCopy, notificationError, notificationSuccess, sendRequest_$POST } from "@/common/function-global";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { customer } from "@/services/customer";
import useSWRMutation from "swr/mutation";
import { statusCode } from "@/common/enum";
import useSWRWithFallbackData from "@/common/use-swr-with-fallback-data";
import { fetcher } from "@/common/const";

const { Item } = Form;

type Props = {
  show: boolean;
  handleModifyCustomerModal: any;
  data: any
};

const ModifyCustomerModal: React.FC<Props> = (props) => {
  const [form] = Form.useForm();

  const [getModifyCustomerData, setModifyCustomerData] = useState<any>({});
  const [getId, setid] = useState<number>(0);

  const {
    data: itemRes,
    error: itemError,
    isLoading,
  } = useSWRWithFallbackData(
    !!getId ? customer().customer().item(getId) : null,
    fetcher,
    { fallbackData: getId }
  );

  const {
    trigger,
    data: customerData,
    error: customerError,
  } = useSWRMutation(customer().customer().modify, sendRequest_$POST);

  const handleFormSubmit = async (values: any) => {
    await trigger({ ...getModifyCustomerData, ...values, birthday: dayjs(values.birthday || null) });
  };


  useEffect(() => {
    if (itemRes?.data) {
      setModifyCustomerData(itemRes.data);
      form.setFieldsValue({ ...getModifyCustomerData, birthday: dayjs(getModifyCustomerData.birthday) });
    }
    if (customerData?.statusCode == statusCode.OK) {
      notificationSuccess("Cập nhật thành công");
      props.handleModifyCustomerModal(customerData.data);
    }
    if (customerError) {
      notificationError(`${customerError}`);
    }
    if (props.show) {
      setid(props?.data.id);
    }
    if (itemError) {
      notificationError(`${itemError}`);
    }
  }, [itemRes, customerData, customerError, props, itemError, form, getModifyCustomerData]);

  const Cancel = () => {
    props.handleModifyCustomerModal();
  }

  return (
    <>
      <Modal
        title={"Cập Nhật Tài Khoản"}
        centered
        open={props.show}
        onOk={() => {
          form.submit();
        }}
        onCancel={Cancel}
        okButtonProps={{
          className: "bg-blue-500",
        }}
        okText="Xác nhận"
        cancelText="Thoát"
      >
        <Form layout="vertical" form={form} onFinish={handleFormSubmit}>
          <Item name="name" label="Tên đầy đủ">
            <Input />
          </Item>
          <Item name="phone" label="Số điện thoại">
            <Input />
          </Item>
          <Item name="point" label="Điểm đã tích lũy">
            <Input disabled={true} />
          </Item>

        </Form>
      </Modal>
    </>
  );
};
export default ModifyCustomerModal;
