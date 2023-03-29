import { Modal } from "antd";
import { order } from "@/services/order";
import { statusCode } from "@/common/enum";
import {
  notificationError,
  notificationSuccess,
} from "@/common/function-global";

type Props = {
  show: boolean;
  handleDeleteOrderModal: any;
  data: any;
};

const DeleteOrderModal: React.FC<Props> = (props) => {
  const handleDeleteOrderModalSubmit = () => {
    fetch(order().delete(props.data.id), {
      method: "DELETE",
    })
      .then((res) => res.json())
      .then((data: any) => {
        if (data?.statusCode == statusCode.OK) {
          notificationSuccess("Xóa thành công");
          props.handleDeleteOrderModal(props.data.index);
        } else {
          notificationError("Xóa thất bại");
        }
      }); // or res.json()
  };

  return (
    <>
      <Modal
        title={"Xóa Đơn Hàng"}
        centered
        open={props.show}
        onOk={handleDeleteOrderModalSubmit}
        onCancel={props.handleDeleteOrderModal}
      >
        <p>Bạn có chắc chắn muốn xoá bản ghi này?</p>
      </Modal>
    </>
  );
};
export default DeleteOrderModal;
