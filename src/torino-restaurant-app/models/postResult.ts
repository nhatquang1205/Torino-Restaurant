export interface IListResultTemplate<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: false;
  hasNextPage: false;
}
