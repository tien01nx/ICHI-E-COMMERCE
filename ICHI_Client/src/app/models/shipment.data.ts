import { ReportDataModel } from './report.data';

export interface ShipmentData {
  id: string;
  carrier_name: string;
  carrier_logo: string;
  carrier_short_name: string;
  service: string;
  expected: string;
  is_apply_only: boolean;
  promotion_id: number;
  discount: number;
  weight_fee: number;
  location_first_fee: number;
  location_step_fee: number;
  remote_area_fee: number;
  oil_fee: number;
  location_fee: number;
  cod_fee: number;
  service_fee: number;
  total_fee: number;
  total_amount: number;
  total_amount_carrier: number;
  total_amount_shop: number;
  price_table_id: number;
  insurrance_fee: number;
  return_fee: number;
  report: ReportDataModel;
}
