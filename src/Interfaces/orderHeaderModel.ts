import { SD_Status } from "../Utility/SD";
import OrderDetail from "./orderDetailsModel";

export default interface orderHeaderModel {
  orderHeaderId: number;
  pickupName: string;
  pickupPhoneNumber: number;
  pickupEmail: string;
  applicatioUserId: string;
  user?: any;
  orderTotal: number;
  orderDate: Date;
  stripePaymentIntentID: string;
  status?: SD_Status;
  totalItems?: number;
  orderDetails?: OrderDetail[];
}
