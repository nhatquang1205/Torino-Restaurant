// utils/flattenParams.ts

type FlattenedParams = { [key: string]: any };

export function customFlatten(obj: any, parentKey = ''): FlattenedParams {
  return Object.keys(obj).reduce(
    (acc: FlattenedParams, key: string): FlattenedParams => {
      const fullKey = parentKey ? `${parentKey}.${key}` : key;
      if (
        typeof obj[key] === 'object' &&
        obj[key] !== null &&
        !Array.isArray(obj[key])
      ) {
        Object.assign(acc, customFlatten(obj[key], fullKey));
      } else {
        acc[fullKey] = obj[key];
      }
      return acc;
    },
    {}
  );
}

export function thoundsandSeperator(x: number | string | undefined | null) {
  const value = x;
  if (value === undefined || value === null) return;

  // Helper function to format a single number
  const formatNumber = (num: number | string): string => {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  };

  // Check if the input is a range
  if (typeof value === 'string' && value.includes('-')) {
    const [start, end] = value.split('-').map((part) => part.trim());
    return `${formatNumber(start)} đ - ${formatNumber(end)} đ`;
  }

  // Otherwise, format it as a single number
  return `${formatNumber(value)} đ`;
}

export function formatDateString(dateString: string) {
  const date = new Date(dateString);
  return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
}

export function reduceStringLengthByAddingEllipsis(x: string) {
  // Random number
  if (x.length < 60) {
    return x;
  }
  const words = x.split(' ');
  if (words.length < 8) {
    words.splice(words.length / 2, 2, '...');
    return words.join(' ');
  }
  const first4Words = words.splice(0, 4);
  const last4Words = words.splice(words.length - 5);

  return `${first4Words.join(' ')} ... ${last4Words.join(' ')}`;
}

export const checkDiscount = (
  dataFrom?: Date | null,
  dataEnd?: Date | null
): boolean => {
  const today = new Date();
  if (dataFrom === null || dataEnd === null) {
    return false;
  }

  if (dataFrom && today < new Date(dataFrom)) {
    return false;
  }

  if (dataEnd && today > new Date(dataEnd)) {
    return false;
  }

  return true;
};

export const convertImageUrlFormat = (image: string | undefined) => {
  if (!image) {
    return '';
  }

  if (image.startsWith('http') || image.startsWith('/')) {
    return image;
  }

  return '/' + image;
};

export const isValidEmail = (email: string) => {
  const isValidEmail = email.match(
    /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
  );
  return isValidEmail;
};

export function DataURIToBlob(dataURI: string) {
  const splitDataURI = dataURI.split(',');
  const byteString =
    splitDataURI[0].indexOf('base64') >= 0
      ? atob(splitDataURI[1])
      : decodeURI(splitDataURI[1]);
  const mimeString = splitDataURI[0].split(':')[1].split(';')[0];

  const ia = new Uint8Array(byteString.length);
  for (let i = 0; i < byteString.length; i++) ia[i] = byteString.charCodeAt(i);

  return new Blob([ia], { type: mimeString });
}
