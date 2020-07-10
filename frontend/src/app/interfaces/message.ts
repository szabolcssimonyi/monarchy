export declare type Action = 'error' | 'success' | 'warning' | 'info';

export interface Message {
    title?: string;
    description: string;
    action?: Action;
    accept?: string;
    cancel?: string;
    showCancel?: boolean;
}