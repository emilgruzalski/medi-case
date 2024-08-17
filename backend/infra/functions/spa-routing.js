// Paths without a file extension are routes of the single page app, so they get index.html.
function handler(event) {
  var request = event.request;
  if (!request.uri.includes('.')) {
    request.uri = '/index.html';
  }
  return request;
}
