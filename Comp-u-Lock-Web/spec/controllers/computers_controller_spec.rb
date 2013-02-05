require 'spec_helper'

describe ComputersController do

  describe "Get 'Index'" do
    it "routes to #index" do
      get("/computer").should route_to("computer#index")
    end
  end

  describe "GET 'edit'" do
    it "routes to #edit" do
      get("/computers/edit").should route_to("computer#edit")
    end
  end

end
