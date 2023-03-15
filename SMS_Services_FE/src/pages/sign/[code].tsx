import { ReactElement, useEffect, useState } from "react";
import Layout from "../layout/layout";
import { NextPageWithLayout } from "../_app";
import React from "react";
import "./sign.module.css";
import SignaturePad from "react-signature-canvas";
import SignatureCanvas from "react-signature-canvas";
import { Button } from "antd";
import useSWRMutation from "swr/mutation";
import { sendRequest_$POST } from "@/common/function-global";

const Sign: NextPageWithLayout = () => {
  const [dataURL, setDataURL] = React.useState<string | null>(null);
  let padRef = React.useRef<SignatureCanvas>(null);

  const [getItem, setItem] = useState<any>({});

  const {
    trigger,
    data: productData,
    error,
  } = useSWRMutation(
    "http://192.168.0.205:7005/api/customer/signature-create",
    sendRequest_$POST
  );

  const handelSubmit = () => {
    const url = padRef.current?.getTrimmedCanvas().toDataURL();
    if (url) setDataURL(url);
    console.log(dataURL);
    setItem({ id: 0, file: dataURL });
    trigger(getItem);
    // fetch("https://localhost:7005/api/customer/signature-create", {
    //   method: "POST",
    //   headers: {
    //     "Content-Type": "application/json",
    //   },
    //   body: JSON.stringify(getItem),
    // });
  };

  const clear = () => {
    padRef.current?.clear();
  };
  return (
    <>
      <div className="d-flex justify-content-center">
        <SignaturePad ref={padRef} canvasProps={{ className: "sigCanvas" }} />
      </div>
      <div className="child">
        <Button onClick={handelSubmit}>Confirm</Button>
        <Button onClick={clear}>Clear</Button>
      </div>
    </>
  );
};

Sign.getLayout = function getLayout(page: ReactElement) {
  return <Layout>{page}</Layout>;
};

export default Sign;
