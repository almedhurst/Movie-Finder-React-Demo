import {useAppSelector} from "../core/store/configureStore";
import {Navigate, useLocation} from "react-router-dom";

interface Props {
    children: JSX.Element;
    performRedirect? : boolean;
}

export default function PrivateComponent({children, performRedirect}: Props){
    const {user} = useAppSelector(state => state.account);
    
    let location = useLocation();
    if(user == null) return performRedirect ?
        (<Navigate to='/login' state={{from: location}} replace />) :
        (<></>)
    
    return children
}