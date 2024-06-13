import {useState} from 'react'
import { useForm,} from "react-hook-form"
import './Login.css'
// import 'bootstrap/dist/css/bootstrap.min.css';
import Registrationimg from "../images/Tablet login-amico.png";
import { Link } from 'react-router-dom';

   
function Login() {
    
  const[ColorChange,setColorChange] = useState<string>('ng-invalid')
  const[ColorChangePassword,setColorChangePassword] = useState<string>("ng-invalid");
 


  const {
      register,
      handleSubmit,
      formState: { errors },
    } = useForm({mode:"onChange",defaultValues:{
      UserName:"",
      Password:"",
    }})

  const  onSubmit = (data:any) => console.log(data)
    
  return (
    <>
     <h2 className='header'>Welcome to View Quick App</h2>
    <img  className='RegisterLogo' src={Registrationimg} alt="" />
    <div  className='containersanju'>
      <h2 className='Register'>Login..</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
    <label className='label'>UserName:</label>
      
      <input className={`input_box  ${ColorChange}` } placeholder=' Name'  {...register("UserName", {required:"Name is require",minLength:{value:3, message:"Minimum Length is 3"},maxLength:{value:20,message:"Maximum Length is 20"},}  )} onKeyUp={()=>{
      errors.UserName?setColorChange('ng-invalid'):setColorChange('ng-valid')  
      }}  /> 
     <br /> 
      {errors.UserName ? <span className='color'>{errors.UserName.message}</span>:<span></span>} 
       <label  className='label'>Password:</label>
      <input type='password' className={`input_box ${ColorChangePassword}`} placeholder='Password' {...register("Password", { required: {value:true,message:"Password is Require.."},
      pattern:{value:/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/,message:"Please Enter Strong Password"}
    })} onKeyUp={()=>errors.Password?setColorChangePassword('ng-invalid'):setColorChangePassword('ng-valid')} />
      <br/>
      {errors.Password && <span className='color'>{errors.Password.message}</span>}
      <p>Don't have an account? <Link to="/Register">Register</Link></p>
      <br />
      <button className='submitbtn'>Submit</button>
      <br />
      <button className='submitbtn'>Forgot-Password</button>
    </form>

    </div>
    </>
  )
}

export default Login
