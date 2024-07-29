import { createSlice } from '@reduxjs/toolkit';

// Define a type for the slice state
export interface AppState {
  isLoading: boolean;
  countLoading: number;
  noLoading: boolean;
  isAuthenticating: boolean;
  isAuthenticated: boolean;
}

// Define the initial state using that type
const initialState: AppState = {
  isLoading: false,
  countLoading: 0,
  noLoading: false,
  isAuthenticating: false,
  isAuthenticated: false,
};

export const appSlice = createSlice({
  name: 'app',
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    setLoading: (state, action) => {
      state.isLoading = action.payload;
    },
    increaseCountLoading: (state) => {
      state.countLoading += 1;
    },
    decreaseCountLoading: (state) => {
      state.countLoading -= 1;
    },
    setIsAuthenticate: (state, action) => {
      state.isAuthenticated = action.payload;
    },
  },
});

export const {
  setLoading,
  increaseCountLoading,
  decreaseCountLoading,
  setIsAuthenticate,
} = appSlice.actions;

export default appSlice.reducer;
