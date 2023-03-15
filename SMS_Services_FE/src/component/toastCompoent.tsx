// import { notification } from 'antd';
import React from 'react';

type NotificationType = 'success' | 'info' | 'warning' | 'error';

type Props = {
    type: NotificationType;
    description: string;
};

// const Toast: React.FC<Props> = (props) => {

//     const [api] = notification.useNotification();   

//     return (<>(

//      api[props.type]({
//         message: 'Thông báo',
//         description: props.description,
//     });
//     )
//     </>)

// }
// export default Toast;

import { notification } from "antd";


const Toast = (type: NotificationType, description: string) => {
    let [api] = notification.useNotification()
    const openNotificationWithIcon = () => {
        api[type]({
            message: 'Thông báo',
            description: description,
        });
    };
    return openNotificationWithIcon;
};

export default Toast;

// toast('success')
// toast('info')
// toast('warning')
// toast('error')