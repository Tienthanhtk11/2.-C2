import { Col, Form, Input, InputNumber, Modal, Row, Select, Table } from "antd";
import { fetcher } from "@/common/const";
import { iconNotification, statusCode } from "@/common/enum";
import { sendRequest_$POST } from "@/common/function-global";
import useSWRWithFallbackData from "@/common/use-swr-with-fallback-data";
import { useEffect, useState } from "react";
import { order } from "@/services/order";
import useSWRMutation from "swr/mutation";
import { ColumnsType } from "antd/es/table";

const { Item } = Form;

type Props = {
  show: boolean;
  handleViewOrderModal: any;
  data: any;
};

interface DataType {
  phone_receive: string;
  message: string;
  telco: string;
  sum_sms: number;
  status: number;
}



const ViewOrderModal: React.FC<Props> = (props) => {
  const columns: ColumnsType<DataType> = [
    {
      title: "STT",
      dataIndex: "index",
      key: "index",
      render: (text, record, index) => <div>{index + 1}</div>,
    },
    {
      title: "Số đt nhận",
      dataIndex: "phone_receive",
      key: "phone_receive",
    },
    {
      title: "Nội dung",
      dataIndex: "message",
      key: "message",
    },
    {
      title: "Nhà mạng",
      dataIndex: "telco",
      key: "telco",
    },

    {
      title: "Số tin nhắn",
      dataIndex: "sum_sms",
      key: "sum_sms",
    },

    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
    },
  ];

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
    data: productData,
    error: productError,
  } = useSWRMutation(order().modify, sendRequest_$POST);

  useEffect(() => {
    if (props?.data) {
      setid(props?.data.id);
    }
    if (itemRes && !error) {
      setModifyOrderData(itemRes.data);
    }
  }, [error, itemRes, props, getId, getModifyOrderData, form]);

  return (
    <>
      <Modal
        title={"Xem thông tin đơn hàng"}
        centered
        open={props.show}
        onCancel={props.handleViewOrderModal}
        onOk={props.handleViewOrderModal}
        width={1000}
      >
        <Row gutter={16}>
          <Col className="gutter-row" span={6}>
            <label>
              Mã đơn hàng: {getModifyOrderData.order_code}
            </label>
          </Col>
          <Col className="gutter-row" span={6}>
            <label>
              ID khách hàng: {getModifyOrderData.customer_id}
            </label>
          </Col>
          <Col className="gutter-row" span={6}>
            <label>
              Ghi chú: {getModifyOrderData.note}
            </label>
          </Col>
          <Col className="gutter-row" span={6}>
            <label>
              Trạng thái: {getModifyOrderData.status}
            </label>
          </Col>
          <Col className="gutter-row" span={6}>
            <label>
              Thời gian gửi: {getModifyOrderData.timeSend}
            </label>
          </Col>
        </Row>
        <Table columns={columns} dataSource={getModifyOrderData.details ?? []} />
      </Modal>
    </>
  );
};
export default ViewOrderModal;
