function batchInsert(items) {
    var context = getContext();
    var container = context.getCollection();
    var count = 0;

    if (!items || !items.length) {
        throw new Error("No items provided");
    }

    var accepted = container.createDocument(
        container.getSelfLink(),
        items[count],
        callback
    );

    if (!accepted) {
        throw new Error("Failed to schedule insert");
    }

    function callback(err, doc) {
        if (err) throw err;

        count++;

        if (count < items.length) {
            var accepted = container.createDocument(
                container.getSelfLink(),
                items[count],
                callback
            );

            if (!accepted) {
                throw new Error("Failed to schedule insert.");
            }
        }
        else {
            context.getResponse().setBody({
                inserted: count
            });
        }
    }
}