
    var i;
    var nodes = new vis.DataSet();
        nodes.add({ id: 3971, label: 'Weight Bearing Capacity', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3972, label: 'Injured', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3973, label: 'Assistance Ability ', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3974, label: 'Assistance Ability ', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3975, label: 'Bear Weight and Ambulate', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3976, label: 'fdsafsdaf ...', shape: 'ellipse', color:'green' });
        nodes.add({ id: 3977, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3978, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3979, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3980, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3981, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3982, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3983, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3984, label: '', shape: 'ellipse', color:'#3280EA' });
        nodes.add({ id: 3985, label: '', shape: 'ellipse', color:'#3280EA' });
    var edges = new vis.DataSet();

        edges.add({from: 3971, to: 3972, label: 'Full', color: '#3280EA' });
        edges.add({from: 3971, to: 3973, label: 'Partial', color: '#3280EA' });
        edges.add({from: 3971, to: 3974, label: 'None', color: '#3280EA' });
        edges.add({from: 3972, to: 3975, label: 'Yes', color: '#3280EA' });
        edges.add({from: 3972, to: 3976, label: 'No', color: '#3280EA' });
        edges.add({from: 3973, to: 3977, label: 'Yes', color: '#3280EA' });
        edges.add({from: 3973, to: 3978, label: 'No', color: '#3280EA' });
        edges.add({from: 3973, to: 3979, label: 'Partially', color: '#3280EA' });
        edges.add({from: 3974, to: 3980, label: 'Full', color: '#3280EA' });
        edges.add({from: 3974, to: 3981, label: 'Partial', color: '#3280EA' });
        edges.add({from: 3974, to: 3982, label: 'None', color: '#3280EA' });
        edges.add({from: 3975, to: 3983, label: 'Yes', color: '#3280EA' });
        edges.add({from: 3975, to: 3984, label: 'No', color: '#3280EA' });
        edges.add({from: 3975, to: 3985, label: 'Partially', color: '#3280EA' });
    // create a network
    var container = document.getElementById('mynetwork');

    // provide the data in the vis format
    var data = {
        nodes: nodes,
        edges: edges
    };
    var options = {
        layout: {
            hierarchical: {
                direction: 'UD',   // UD, DU, LR, RL
                sortMethod: 'directed' // hubsize, directed
            }
        },
        physics: {
            enabled: false
        }
        //physics: {
        //    enabled: true,
        //    hierarchicalRepulsion: {
        //        centralGravity: 0.0,
        //        springLength: 100,
        //        springConstant: 0.01,
        //        nodeDistance: 170,
        //        damping: 0.09
        //    }
        //}
    };

    // initialize network
    var network = new vis.Network(container, data, options);

    network.on("selectNode", function (params) {
        console.log('selectNode Event:', params);
        $("<input type='hidden' id='ParentNodeID' name='ParentNodeID' value="+params.nodes+">").insertAfter("#button");
    });
