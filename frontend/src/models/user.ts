export interface RestaurantSummary {
    id: number;
    name: string;
}

export interface User {
    userId: string;
    authenticationToken: string;
    availableRestaurants: RestaurantSummary[];
}

export interface SelectRestaurantDto {
    restaurantId: number;
}

export interface ContextToken {
    token: string;
}