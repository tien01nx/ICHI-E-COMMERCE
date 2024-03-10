export const Environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7150/api',
  apiBaseRoot: 'https://localhost:7150',
  contentTypeJson: 'application/json',
  contentMultiPart: 'multipart/form-data',
  statusSuccess: 'badge badge-phoenix fs-10 badge-phoenix-success',
  statusWarning: 'badge badge-phoenix fs-10 badge-phoenix-warning',
  // tạo list quyền cho user để hiện thị ra select option với name là tên quyền: USER, ADMIN, EMPOYEE
  roles: [{ name: 'USER' }, { name: 'ADMIN' }, { name: 'EMPLOYEE' }],
};
