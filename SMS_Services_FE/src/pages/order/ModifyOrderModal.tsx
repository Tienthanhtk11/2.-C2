import { Form, Input, InputNumber, Modal, Select } from "antd";
import { fetcher } from "@/common/const";
import { iconNotification, statusCode } from "@/common/enum";
import {
  notificationError,
  notificationSuccess,
  sendRequest_$POST,
} from "@/common/function-global";
import useSWRWithFallbackData from "@/common/use-swr-with-fallback-data";
import { useEffect, useState } from "react";
import { order } from "@/services/order";
import useSWRMutation from "swr/mutation";

const { Item } = Form;

type Props = {
  show: boolean;
  handleModifyOrderModal: any;
  data: any;
};

const ModifyOrderModal: React.FC<Props> = (props) => {
  const [form] = Form.useForm();
  const [getModifyOrderData, setModifyOrderData] = useState<any>({});
  const [getId, setid] = useState<number>(0);

  const {
    data: itemRes,
    error,
    isLoading,
  } = useSWRWithFallbackData(!!getId ? order().item(getId) : null, fetcher, {
    fallbackData: getId,
  });

  const {
    trigger,
    data: orderData,
    error: orderError,
  } = useSWRMutation(order().modify, sendRequest_$POST);

  const handleFormSubmit = async (values: any) => {
    trigger({ ...getModifyOrderData, ...values });
  };

  const lstStatus: any = [
    { value: 0, label: "Khởi tạo" },
    { value: 1, label: "Đã hoàn thành" },
  ];

  useEffect(() => {
    if (props?.data) {
      setid(props?.data.id);
    }
    if (itemRes && !error) {
      setModifyOrderData(itemRes.data);
      form.setFieldsValue(getModifyOrderData);
    }
  }, [error, itemRes, props, getId, getModifyOrderData, form]);

  useEffect(() => {
    if (orderData?.statusCode == statusCode.OK) {
      notificationSuccess("Cập nhật thành công");
      props.handleModifyOrderModal(orderData?.data);
    }
    if (orderError) {
      notificationError(`${orderError}`);
    }
  }, [orderData, orderError]);

  return (
    <>
      <Modal
        title={"Cập Nhật Đơn Hàng"}
        centered
        open={props.show}
        onOk={() => {
          form.submit();
        }}
        onCancel={props.handleModifyOrderModal}
      >
        <Form layout="vertical" form={form} onFinish={handleFormSubmit}>
          <Item name="status" label="Trạng thái thực hiện">
            <Select className="w-100" options={lstStatus} />
          </Item>
        </Form>
      </Modal>
    </>
  );
};
export default ModifyOrderModal;
