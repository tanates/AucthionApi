export default class AuctionItem {
    constructor(id, name, startPrice, endPrice, startTime, endTime, description, idAuctione) {
        this.id = id;
        this.name = name;
        this.startPrice = startPrice;
        this.endPrice = endPrice;
        this.startTime = startTime;
        this.endTime = endTime;
        this.description = description;
        this.idAuctione = idAuctione;
    }
}