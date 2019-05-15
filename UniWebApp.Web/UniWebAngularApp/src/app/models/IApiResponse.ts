export interface IApiResponse {
  status: number;
  message: string;
}

export interface IApiResponseWithReturn<T> extends IApiResponse {
  return: T;
}
