import Dexie from "./dexie";
class MyDB extends Dexie {
    constructor() {
        super("MyDB");
        this.version(1).stores({
            friends: "++id,name,age"
        });
    }
}
var db = new MyDB();
db.friends.add({ name: "Kalle", age: 23 }).then(() => {
    db.friends.where('age').below(30).count(count => {
        console.log("Num yound friends: " + count);
    });
});
//# sourceMappingURL=dexie-tests.js.map