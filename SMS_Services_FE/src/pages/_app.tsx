import "/node_modules/bootstrap/dist/css/bootstrap.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import "@/styles/globals.css";
import { NextPage } from "next";
import type { AppProps } from "next/app";
import { ReactElement, ReactNode, useEffect, useState } from "react";
import { Provider } from "react-redux";
import { legacy_createStore as createStore } from "redux";
import { allReducers } from "../store/reducers/index_reducers";
import { useRouter } from "next/router";
// import RegisterModal from "./layout/navbar/registerModal";

const store = createStore(allReducers);

export type NextPageWithLayout<P = {}, IP = P> = NextPage<P, IP> & {
  getLayout?: (page: ReactElement) => ReactNode;
};

type AppPropsWithLayout = AppProps & {
  Component: NextPageWithLayout;
};

export default function App({ Component, pageProps }: AppPropsWithLayout) {
  const router = useRouter();

  const [isVisibleRegisterModal, setVisibleRegisterModal] = useState(false);

  const [idAffliate, setIdAffliate] = useState<any>();


  // đăng ký

  const handleOpenRegisterModal = () => {
    setVisibleRegisterModal(true);
  };

  const handleRegisterModalClose = () => {
    localStorage.removeItem('affliate');
    setVisibleRegisterModal(false);
  };

  useEffect(() => {
    require("/node_modules/bootstrap/dist/js/bootstrap.bundle.min.js");
    (router.route == '/payment/VnPayIPN') && router.push({ pathname: router.route, query: router.query }, undefined, { shallow: true });
    // setIdAffliate(`${localStorage.getItem('affliate')}`)   
    console.log(idAffliate);
  }, []);

  useEffect(() => {
    const affliate = localStorage.getItem('affliate')
    setIdAffliate(affliate)
    if (!!idAffliate) {
      handleOpenRegisterModal();
    }
  });

  const getLayout = Component.getLayout ?? ((page) => page);

  return (
    <Provider store={store}>
      {getLayout(<Component {...pageProps} />)}

      {/* <RegisterModal
        show={isVisibleRegisterModal}
        handleRegisterModalClose={
          handleRegisterModalClose
        }
        data={{ customer_affliate: idAffliate }}
      /> */}
    </Provider>
  )
}