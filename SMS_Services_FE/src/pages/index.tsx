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
import { useDispatch, useSelector } from "react-redux";
import { useRouter } from "next/router";
import SMS from "./sms";
import { clear_info_current_user } from "@/store/action/info_current_user_action";
import { clear_info_current_admin_user } from "@/store/action/info_current_user_admin_action";
import { typeUser_delete } from "@/store/action/type_user_action";
import { userType } from "@/common/enum";

const { Header, Sider, Content } = Layout;

type ContainerProps = {
  children: React.ReactNode;
};

export default function Admin() {
  const router = useRouter();
  const [collapsed, setCollapsed] = useState(false);
  const [getListMenu, setListMenu] = useState([]);
  const [getTypeUser, setTypeUser] = useState('');  
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  // get info user
  const getStore = useSelector(
    (state: any) => {
      return {
        admin: state.infoCurrentUserAminReducers,
        customer: state.infoCurrentUserReducers,
        typeuser: state.typeUserReducers
      }
    }
  );

  const [getDefaultSelectedKeys, setDefaultSelectedKeys] = useState(getStore.typeuser == userType.admin ? '2' : '4');
  const [selectedMenuItem, setSelectedMenuItem] = useState(getStore.typeuser == userType.admin ? '2' : '4');


  const dispatch = useDispatch();

  const handleLogout = () => {
    dispatch(clear_info_current_user())
    dispatch(clear_info_current_admin_user())
    dispatch(typeUser_delete())
  }

  const items: MenuProps["items"] = [
    {
      label: (
        <a href="" style={{ textDecoration: "none" }} onClick={() => handleLogout()}>
          Đăng xuất
        </a>
      ),
      key: "0",
    },
  ];

  useEffect(() => {
    if (!getStore.admin?.token && !getStore.customer?.token) {
      router.push("/login/customer");
    }
    if (getStore?.typeuser) {
      setTypeUser(getStore.typeuser)
      // setDefaultSelectedKeys(3);
      let menu: any = [
        {
          key: "1",
          icon: <VideoCameraOutlined />,
          label: "Đơn hàng",
          active: ''
        },
        {
          key: "2",
          icon: <VideoCameraOutlined />,
          label: "Tài khoản admin",
          active: ''
        },
        {
          key: "3",
          icon: <VideoCameraOutlined />,
          label: "Khách hàng",
          active: ''
        },
        {
          key: "4",
          icon: <VideoCameraOutlined />,
          label: "Tin đã nhận",
          active: userType.customer,
        }
      ]
      if (getTypeUser == userType.admin) {
        let mapMenu = menu.map((obj:any) => {
          delete obj.active;
          return obj
        });
        setListMenu(mapMenu);
        // setSelectedMenuItem('2')
      }
      else {
        let mapMenu = menu.map((obj:any) => {
          if (obj?.active == getTypeUser) {
            delete obj.active;
            return obj
          }
        });
        setListMenu(mapMenu);
        // setSelectedMenuItem('4')
      }
    }
  }, [getStore, router]);

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
        {/* getTypeUser == userType.admin ? ["2"] : ["4"] */}
        <Menu
          theme="dark"
          mode="inline"
          defaultSelectedKeys={[`${getDefaultSelectedKeys}`]}
          onClick={(e) => {
            setSelectedMenuItem(e.key)
          }}
          items={getListMenu}
        />
      </Sider>
      <Layout className="site-layout">
        <Header
          className="d-flex justify-content-between"
          style={{ padding: 0, background: colorBgContainer }}>
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
                {(getStore.typeuser == userType.admin) ? getStore.admin.name : getStore.customer.name}
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
