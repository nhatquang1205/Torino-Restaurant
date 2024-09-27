export interface IProductModel {
  id: number;
  name: string;
  description: string;
  vietnameseDescription: string;
  categoryId: number;
  category: string;
  costPrice: number;
  price: number;
  isUseForPrinter: boolean;
  slug: string;
  saleCount: number;
  imageUrl: string;
  productPrices: IProductPriceModel[];
}

export interface IProductPriceModel {
  id?: number;
  name: string;
  price: number;
  display_price: string;
}
