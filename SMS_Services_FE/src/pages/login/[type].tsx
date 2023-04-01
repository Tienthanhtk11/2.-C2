import React, { useEffect, useState } from "react";
import { Button, Checkbox, Form, Input, Layout } from "antd";
import style from "../login/login.module.css";
import { useDispatch, useSelector } from "react-redux";
import { useRouter } from "next/router";
import useSWRMutation from "swr/mutation";
import { notificationError, notificationSuccess, sendRequestLogin_$POST } from "@/common/function-global";
import { user } from "@/services/user";
import { add_info_current_admin_user, clear_info_current_admin_user } from "@/store/action/info_current_user_admin_action";
import { statusCode, userType } from "@/common/enum";
import { customer } from "@/services/customer";
import { add_info_current_user, clear_info_current_user } from "@/store/action/info_current_user_action";
import { typeUser_add } from "@/store/action/type_user_action";

export default function Login() {
  const router = useRouter();
  const [getTypeLogin, setTypeLogin] = useState('');
  // get user admin
  const getStore = useSelector(
    (state: any) => {
      return {
        admin: state.infoCurrentUserAminReducers,
        customer: state.infoCurrentUserReducers
      }
    }
  );
  const dispatch = useDispatch();

  const {
    trigger: triggerAdmin,
    data: adminLoginData,
    error: errorAdmin,
  } = useSWRMutation(user().login, sendRequestLogin_$POST);

  const {
    trigger: triggerCustomer,
    data: customerLoginData,
    error: errorCustomer,
  } = useSWRMutation(customer().customer().login, sendRequestLogin_$POST);

  const handleFormSubmit = async (values: any) => {
    let datapush: any = {
      user_name: values.user_name,
      password: values.password,
    };
    if (getTypeLogin == userType.admin) {
      triggerAdmin({
        ...datapush,
      });
      dispatch(clear_info_current_user())
      dispatch(typeUser_add(userType.admin))
    }
    else {
      triggerCustomer({
        ...datapush,
      });
      dispatch(clear_info_current_admin_user())
      dispatch(typeUser_add(userType.customer))
    }
  };

  useEffect(() => {
    if (router.isReady) {
      setTypeLogin(`${router.query.type}`);
    }
    if (adminLoginData?.statusCode == statusCode.OK) {
      notificationSuccess(adminLoginData.message);
      dispatch(add_info_current_admin_user(adminLoginData.data));
      router.push("/");
    }
    if ((!!adminLoginData) && adminLoginData?.statusCode == statusCode.Error) {
      notificationError(adminLoginData?.message);
    }
    if (customerLoginData?.statusCode == statusCode.OK) {
      notificationSuccess(customerLoginData.message);
      dispatch(add_info_current_user(customerLoginData.data));
      router.push("/");
    }
    if ((!!customerLoginData) && customerLoginData?.statusCode == statusCode.Error) {
      notificationError(customerLoginData?.message);
    }    
  }, [router.isReady,adminLoginData, customerLoginData]);
  // router.isReady, adminLoginData, getStore, customerLoginData
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

