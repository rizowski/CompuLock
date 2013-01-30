class AccountHistory < ActiveRecord::Base
  attr_accessible :domain, :last_visited, :url, :visit_count

  validates :domain, :presence => true
  

  belongs_to :account
end
