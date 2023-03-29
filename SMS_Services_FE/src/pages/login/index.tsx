import React, { useEffect } from "react";
import { Button, Checkbox, Form, Input, Layout } from "antd";
import style from "../login/login.module.css";
import { useDispatch, useSelector } from "react-redux";
import { useRouter } from "next/router";
import useSWRMutation from "swr/mutation";
import { sendRequest_$POST } from "@/common/function-global";
import { user } from "@/services/user";
import { add_info_current_admin_user } from "@/store/action/info_current_user_admin_action";

export default function Login() {
  const router = useRouter();

  // get user admin
  const getInfoCurrentUserAdmin = useSelector(
    (state: any) => state.infoCurrentUserAminReducers
  );
  const dispatch = useDispatch();

  const {
    trigger,
    data: adminLoginData,
    error,
  } = useSWRMutation(user().login, sendRequest_$POST);

  const handleFormSubmit = async (values: any) => {
    let datapush: any = {
      user_name: values.user_name,
      password: values.password,
    };
    trigger({
      ...datapush,
    });
  };
  useEffect(() => {
    if (adminLoginData?.data) {
      dispatch(add_info_current_admin_user(adminLoginData.data));
    }
    if (getInfoCurrentUserAdmin?.token) {
      router.push("/");
    }
  }, [adminLoginData, getInfoCurrentUserAdmin]);
  return (
    <>
      <Layout className={`${style.layout}`}>
        <Form
          className={`${style.pc}`}
          name="basic"
          labelCol={{ span: 6 }}
          wrapperCol={{ span: 16 }}
          style={{ maxWidth: 600, background: "#fff" }}
          initialValues={{ remember: true }}
          onFinish={handleFormSubmit}
          autoComplete="off"
        >
          <Form.Item wrapperCol={{ span: 16 }} className={`${style.colh3}`}>
            <h3 className={`text-center ${style.h3}`}>Đăng nhập</h3>
          </Form.Item>
          <Form.Item
            label="Tài khoản"
            name="user_name"
            rules={[
              { required: true, message: "Vui lòng nhập tài khoản của bạn!" },
            ]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            label="Mật khẩu"
            name="password"
            rules={[
              { required: true, message: "Vui lòng nhập mật khẩu của bạn!" },
            ]}
          >
            <Input.Password />
          </Form.Item>

          {/* <Form.Item
            valuePropName="checked"
            wrapperCol={{ span: 16 }}
            className={`${style.col}`}
          >
            <div className={`d-flex justify-content-between`}>
              <Checkbox>Nhớ mật khẩu</Checkbox>
              {/* <p><a href="" className=" text-decoration-none">Quên mật khẩu</a></p> */}
          {/* </div>
      </Form.Item> */}

          <Form.Item wrapperCol={{ span: 16 }} className={`${style.col}`}>
            <Button type="primary" htmlType="submit" className={`w-100`}>
              Đăng nhập
            </Button>
          </Form.Item>
        </Form>
      </Layout >
    </>
  );
}
