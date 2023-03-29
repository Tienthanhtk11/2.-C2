import React, { useEffect, useState } from "react";
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  UserOutlined,
  VideoCameraOutlined,
} from "@ant-design/icons";
import { Avatar, Dropdown, Layout, Menu, MenuProps, Space, theme } from "antd";
import Order from "./order";
import User from "./user";
import Customer from "./customer";
import { DownOutlined } from "@ant-design/icons";
import { useSelector } from "react-redux";
import { useRouter } from "next/router";
import SMS from "./sms";

const { Header, Sider, Content } = Layout;

type ContainerProps = {
  children: React.ReactNode;
};

export default function Admin() {
  const router = useRouter();
  const [getItem, setItem] = useState<any>({});
  const [collapsed, setCollapsed] = useState(false);
  const [selectedMenuItem, setSelectedMenuItem] = useState("1");
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  // get info user
  const getInfoCurrentUserAdmin = useSelector(
    (state: any) => state.infoCurrentUserAminReducers
  );

  const items: MenuProps["items"] = [
    {
      label: (
        <a href="" style={{ textDecoration: "none" }}>
          Logout
        </a>
      ),
      key: "0",
    },
  ];

  useEffect(() => {
    if (!getInfoCurrentUserAdmin?.token) {
      router.push("/login");
    }
  }, [getInfoCurrentUserAdmin, router]);

  const componentsSwtich = (key: any) => {
    switch (key) {
      case "1":
        return <Order />;
      case "2":
        return <User />;
      case "3":
        return <Customer />;
      case "4":
        return <SMS />;
      default:
        break;
    }
  };

  return (
    <Layout style={{ height: "100vh" }}>
      <Sider trigger={null} collapsible collapsed={collapsed}>
        <Menu
          theme="dark"
          mode="inline"
          defaultSelectedKeys={["2"]}
          onClick={(e) => setSelectedMenuItem(e.key)}
          items={[
            {
              key: "1",
              icon: <VideoCameraOutlined />,
              label: "Đơn hàng",
            },
            {
              key: "2",
              icon: <VideoCameraOutlined />,
              label: "Tài khoản admin",
            },
            {
              key: "3",
              icon: <VideoCameraOutlined />,
              label: "Khách hàng",
            }
            ,
            {
              key: "4",
              icon: <VideoCameraOutlined />,
              label: "Tin đã nhận",
            }
          ]}
        />
      </Sider>
      <Layout className="site-layout">
        <Header style={{ padding: 0, background: colorBgContainer }}>
          {React.createElement(
            collapsed ? MenuUnfoldOutlined : MenuFoldOutlined,
            {
              className: "trigger",
              onClick: () => setCollapsed(!collapsed),
            }
          )}
          <Dropdown className="mx-3" menu={{ items }} trigger={["click"]}>
            <a
              onClick={(e) => e.preventDefault()}
              style={{ color: "#000", textDecoration: "none" }}
            >
              <Space>
                <Avatar
                  style={{ backgroundColor: "#0984e3" }}
                  icon={<UserOutlined />}
                />
                {getInfoCurrentUserAdmin.full_name}
                <DownOutlined />
              </Space>
            </a>
          </Dropdown>
        </Header>
        <Content
          style={{
            margin: "24px 16px",
            padding: 24,
            minHeight: 280,
            background: colorBgContainer,
          }}
        >
          {componentsSwtich(selectedMenuItem)}
        </Content>
      </Layout>
    </Layout>
  );
}
