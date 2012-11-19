/// <reference path="jquery.d.ts" />
interface Hub {
    id: string;
    state: any;
    start(options?: any, callback?: () => any): JQueryPromise;
    clearProxy(): void;
    recreateProxy(): void;
}

interface SignalR {
    events: any;

    log(msg: string, logging: bool): void;
    isCrossDomain(url: string): bool;
    changeState(connection: any, expectedState: number, newState: number): bool;
    isDisconnecting(connection: any): bool;

    hub: Hub;
    connection: HubConnection;

    init(url, qs, logging): void;
    ajaxDataType: string;
    logging: bool;
    reconnectDelay: number;
    state: any;
    start(options?: any, callback?: () => any): JQueryPromise;
    starting(callback?: () => any): SignalR;
    send (data): SignalR;
    sending (callback?: () => any): SignalR;
    received (callback?: (data) => any): SignalR;
    stateChanged (callback?: (data) => any): SignalR;
    error (callback?: (data) => any): SignalR;
    disconnected (callback?: () => any): SignalR;
    reconnected (callback?: () => any): SignalR;
    stop (async? : bool): SignalR;
}

interface HubConnection extends SignalR {
    hub: Hub;
}

// extend JQuery interface
interface JQueryStatic {
    signalR: SignalR;
    connection: SignalR;
    extendSignalR: () => void;
}