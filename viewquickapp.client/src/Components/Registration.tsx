import { useRef, useState } from "react";
import { useForm } from "react-hook-form";
import "./Registration.css";
import Registrationimg from "../images/Sign up-amico.png";
import { Link } from "react-router-dom";


function Registration() {
  const [ColorChange, setColorChange] = useState<string>("ng-invalid");
  const [ColorChangeEmail, setColorChangeEmail] =
    useState<string>("ng-invalid");
  const [ColorChangePassword, setColorChangePassword] =
    useState<string>("ng-invalid");
  const [ColorChangeConfirmPass, setColorChangeConfirmPass] =
    useState<string>("ng-invalid");

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm({
    mode: "onChange",
    defaultValues: {
      UserName: "",
      Email: "",
      Password: "",
      ConfirmPassword: "",
    },
  });

  const Password = useRef("");
  Password.current = watch("Password", "");

  const onSubmit = (data: any) => console.log(data);

  return (
    <>
      <h2 className="headerSanju">Welcome to View Quick App</h2>
      <img className="RegisterLogo" src={Registrationimg} alt="" />
      <div className="containersanju" style={{ marginTop: "37px" }}>
        <h2 className="Register">Registration:</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <label className="label">UserName:</label>

          <input
            className={`input_box  ${ColorChange}`}
            placeholder=" Name"
            {...register("UserName", {
              required: "Name is require",
              minLength: { value: 3, message: "Minimum Length is 3" },
              maxLength: { value: 20, message: "Maximum Length is 20" },
            })}
            onKeyUp={() => {
              errors.UserName
                ? setColorChange("ng-invalid")
                : setColorChange("ng-valid");
            }}
          />
          <br />
          {errors.UserName ? (
            <span className="color">{errors.UserName.message}</span>
          ) : (
            <span></span>
          )}

          <label className="label">Email:</label>
          <input
            type="text"
            className={`input_box ${ColorChangeEmail}`}
            placeholder="Email"
            {...register("Email", {
              required: { value: true, message: "Email is Require.." },
              pattern: {
                value: /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/,
                message: "Email pattern is incorrect..",
              },
            })}
            onKeyUp={() =>
              errors.Email
                ? setColorChangeEmail("ng-invalid")
                : setColorChangeEmail("ng-valid")
            }
          />
          <br />
          {errors.Email && (
            <span className="color">{errors.Email.message}</span>
          )}

          <label className="label">Password:</label>
          <input
            type="password"
            className={`input_box ${ColorChangePassword}`}
            placeholder="Password"
            {...register("Password", {
              required: { value: true, message: "Password is Require.." },
              pattern: {
                value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/,
                message: "Please Enter Strong Password",
              },
            })}
            onKeyUp={() =>
              errors.Password
                ? setColorChangePassword("ng-invalid")
                : setColorChangePassword("ng-valid")
            }
          />
          <br />
          {errors.Password && (
            <span className="color">{errors.Password.message}</span>
          )}

          <label className="label">ConfirmPassword:</label>
          <input
            type="password"
            className={`input_box ${ColorChangeConfirmPass}`}
            placeholder="ConfirmPassword"
            {...register("ConfirmPassword", {
              required: {
                value: true,
                message: "ConfirmPassword is Require..",
              },
              pattern: {
                value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/,
                message: "Please Enter Strong Password",
              },
              validate: (value) =>
                value === Password.current || "The passwords do not match",
            })}
            onKeyUp={() =>
              errors.ConfirmPassword
                ? setColorChangeConfirmPass("ng-invalid")
                : setColorChangeConfirmPass("ng-valid")
            }
          />
          <br />
          {errors.ConfirmPassword && (
            <span className="color">{errors.ConfirmPassword.message}</span>
          )}
          <p>
            already Register? <Link to="/login">Login</Link>
          </p>
          <button className="submitbtn">Submit</button>
          <br />
        </form>
      </div>
    </>
  );
}

export default Registration;
