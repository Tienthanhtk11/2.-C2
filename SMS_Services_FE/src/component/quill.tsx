import React, { useMemo, useState } from "react";
import "react-quill/dist/quill.snow.css";
import dynamic from "next/dynamic";
import ReactQuill from "react-quill";
import Script from "next/script";

const fonts: any = [];

const formats = [
  "header",
  "font",
  "size",
  "bold",
  "italic",
  "underline",
  "strike",
  "blockquote",
  "list",
  "bullet",
  "indent",
  "link",
  "image",
  "video",
];

const modules = {
  toolbar: [
    ["bold", "italic", "underline", "strike"], // toggled buttons
    ["blockquote", "code-block"],
    [{ header: 1 }, { header: 2 }], // custom button values
    [{ list: "ordered" }, { list: "bullet" }],
    [{ script: "sub" }, { script: "super" }], // superscript/subscript
    [{ indent: "-1" }, { indent: "+1" }], // outdent/indent
    [{ direction: "rtl" }], // text direction

    [{ size: ["small", false, "large", "huge"] }], // custom dropdown
    [{ header: [1, 2, 3, 4, 5, 6, false] }],

    [{ color: [] }, { background: [] }], // dropdown with defaults from theme
    [{ font: [] }],
    [{ align: [] }],
    ["image", "video"],
    ["clean"], // remove formatting button
  ],
};

interface OnChangeHandler {
  (e: any): void;
}

type Props = {
  value: string;
  placeholder: string;
  onChange: OnChangeHandler;
};

const QuillEditor: React.FC<Props> = ({ value, onChange, placeholder }) => {
  const ReactQuill = useMemo(
    () => dynamic(() => import("react-quill"), { ssr: false }),
    []
  );
  const [editorHtml, setEditorHtml] = useState("");

  const handleChange = (html: any) => {
    setEditorHtml(html);
  };

  return (
    <>
      <ReactQuill
        value={value || ""}
        modules={modules}
        formats={formats}
        onChange={onChange}
        placeholder={placeholder}
      />
    </>
  );
};

export default QuillEditor;
