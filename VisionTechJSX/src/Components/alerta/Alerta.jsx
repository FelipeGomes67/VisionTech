import Swal from "sweetalert2";
import "./Alerta.css";

export const Alerta = ({
  title,
  text,
  icon,
  showCancelButton = null,
  confirmButtonText = null,
  cancelButtonText = null,
  confirmButtonColor = "#3085d6",
  cancelButtonColor = "#d33",
  input = null,
  inputLabel = null,
  inputValue = null,
  inputValidator = null,
  inputOptions = null,
}) => {
  return Swal.fire({
    title,
    text,
    icon,
    showCancelButton,
    confirmButtonText,
    cancelButtonText,
    confirmButtonColor,
    cancelButtonColor,
    input,
    inputLabel,
    inputValue,
    inputValidator,
    inputOptions,
  });
};