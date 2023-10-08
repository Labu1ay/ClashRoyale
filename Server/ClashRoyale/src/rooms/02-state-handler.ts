import { Room, Client, Delayed } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";
import axios from "axios";
import { Library } from "../Library";

export class Player extends Schema {
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;
    playersDeck = new Map();

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            //this.state.movePlayer(client.sessionId, data);
        });
    }

    gameIsStarted: boolean = false;
    awaitStart: Delayed;
    async onJoin (client: Client, data) {
        try {
            const responce = await axios.post(Library.getDeckUri, {key: Library.phpKEY, userID: data.id});
            console.log(responce.data);
            this.playersDeck.set(client.id, responce.data);
        } catch (error) {
            console.log('Вылетела ошибка' + error);
            
        }
        this.state.createPlayer(client.sessionId);

        //if(this.clients.length < 2) return;

        this.broadcast("GetReady");
        this.awaitStart = this.clock.setTimeout(() => {
            try {
                this.broadcast("Start", JSON.stringify({player1ID: this.clients[0].id, player1: this.playersDeck.get(this.clients[0].id), player2: this.playersDeck.get(this.clients[0].id)}));
                this.gameIsStarted = true;
            } catch (error) {
                this.broadcast("CancelStart");
            }
        }, 1000);

        
    }

    onLeave (client) {
        if(this.gameIsStarted === false && this.awaitStart !== undefined && this.awaitStart.active){
            this.broadcast("CancelStart");
            this.awaitStart.clear(); 
        }

        if(this.playersDeck.has(client.id)) 
            this.playersDeck.delete(client.id);
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        
    }

}
